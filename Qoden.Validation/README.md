# Validation #

## Rationale for yet another data validation library ##

I want easy and flexible validation library where I don't have to write ton's of code to implement custom validations.
I want to use same validation code to check for app errors and validate arguments and check state conditions.

## Sample use ##
Complex property
```
public string PhoneNumber { 
	set {
		var val = (value ?? "").Trim().Replace("-","").Replace("(", "").Replace(")","").Replace(" ", "");
		//Check val is NotEmpty and has minimum length of 6.
		//If check is failed it is saved in Errors array for later use.
		//Depending on Errors configuration these checks throw exception right away or collect them
		//in a list.
		if (Errors.CheckValue (val).NotEmpty ().MinLength(6)) {
			var phoneUtils = PhoneNumberUtil.GetInstance ();
			var username = String.Format ("+{0} {1}", CountryCode, value);
			try {
				var phoneNumber = phoneUtils.Parse (username, "ZZ");
				Username = phoneNumber;
			} catch (NumberParseException) {
				Errors.Add (new UsernameError {
					Value = username
				});
			}
		}
	}
}
```
Simple property
```
public DateTime Start { 	
	set { 
		var val = value.UserToUTC ();
		//Check supplied date time 
		Errors.CheckValue (val).LessThan (End);
		//If checks successfull then perform setter action
		if (!Errors.HasErrorsForKey()) {
			var diff = End - Start;
			eventData = val;
			End = Start + diff;
		}
	}
}
```

## Design ##

### Error ###

Errors are represented as subclasses of Error which in turn subclass of Exception. This design allow to write event handling code
either as try-catch blocks or as generic code which inspect object properties to figure out what happened. Error contains error context
information, which usually contains actual and expected values and other data.

Error messages are generated with NamedFormat utility which get object and format string like "{Prop1} = {Prop2}" and
generate a message with placeholders filled in with property/key values. When error messages are generated error object act as a context 
for error messsage. This means that if error object has properties Key, ActualValue, ExpectedValue it does makes sense to 
have a message "{Key} has value {ActualValue} but expected is {ExpectedValue}".

### Validator ###

Error list object collect errors or to be used by UI later on. ErrorList could be configured to throw errors immediately 
instead of storing them.

### Check<T> and new Validation methods ###

Check struct store validation state during validation. You create new validator methods by making extension methods for Check<T>.
Check<T> contains error list and methods to add errors to it.
Example:

```
public static class NumberValidation
{
	public const string MoreThanMessage = "{Key} expected to be more than {Min}";

	public static Check<T> MoreThan<T> (this Check<T> check, T expected, string message = MoreThanMessage)
		where T : struct, IComparable
	{
		if (check.Value.CompareTo (expected) < 0) {
			check.Fail (new RangeError<T> {
				MessageFormat = message,
				Min = expected
			});
		}
		return check;
	}
}
```
This validator is later used like this:
```
void MyMethod(int value)
{
  Errors.Check(value).MoreThan(10, "My custom error message");
}
```
Note how generic parameter constraints is used to make this method applicable only to Check<T> instantiated 
with T which is comparable value type.

There is also special Convert extension method which convert Check<T> to Check<U>. Example:
```
public string CountryCode { 
	get { return Username.CountryCode.ToString(); }
	set { 
		var check = Errors.CheckValue (value)
			.NotNull ()
			.ConvertTo<Int16> ()
			.MoreThan<Int16> (0);				
		Username = Username.ToBuilder ().SetCountryCode (check.Value).Build ();				
	}
}
```
Note how Check<string> converted to Check<int> and then value is used to set country code.

## Existing validators and error objects ##

There are lot of them defined in *Validation files. Please go over these files.