# DynamicAccessor

This is a personal library made to make reflection based code easier.

## DynamicAccessor can do the following operations:
- Access non-public properties and fields
- Set non-public properties and fields
- Handle type conversion when casting from an accessed value
- Find and invoke non-public methods
- Find and invoke non-public generic methods.

# Limitations
- If something is defined in a derived class and base class DynamicAccessor will access only the derived class version

# Installation

[![NuGet version (DynamicAccessor )](https://img.shields.io/nuget/v/DynamicAccessor.svg?style=flat-square)](https://www.nuget.org/packages/DynamicAccessor/)

# Useage

## Example setup:
```cs
// A test class with a non-public property and method
class TestClass
{
    // Test private property
    private string PrivateProperty = "Some Value";

    // Test method
    private int GetSomethingPrivate(int i) => i;

    // Test generic method
    private Type GetSomethingGeneric<T>() => typeof(T);
}

TestClass testClass = new TestClass();

// Create a new DynamicObjectAccessor around the object you are trying to access
// Using a inbuilt reflection cashe for multiple lookups
DynamicObjectAccessor accessor = DynamicObjectAccessor.Create(testClass, useCache: true);
```

## Accessing non-public values
```cs
// Simply access the property as you would a any public property
DynamicObjectAccessor wrappedValue =  accessor.PrivateProperty;

// When you wish to use the value simply cast to valid type
string value = (string) wrappedValue;
// value == "Some Value"
```

## Setting non-public values
```cs
// Simply assign to the property the value you wish, you can use derived types
accessor.PrivateProperty = "Some New Value";
// value == "Some New Value"
```

## Indexers
DynamicAccessor supports accessing object via single and multi-indexers
```cs
// Accesing a value
DynamicObjectAccessor wrappedValue =  accessor["PrivateProperty"];
// Setting a value
accessor["PrivateProperty"] = "Some New Value";

// Multi-indexing
DynamicObjectAccessor wrappedDeepObjectValue =  deepObjectyAccessor["SomeField", "SomeFieldIndexChild"];
```

## Invoke method
```cs
// You can access the DynamicMethodAccessor or just invoke it directly like any other method
DynamicMethodAccessor methodAccessor = accessor.GetSomethingPrivate;
int value = (int) methodAccessor(42);
// value == 42
// OR
int value = (int) accessor.GetSomethingPrivate(42);
// value == 42
```

## Invoke generic methods
```cs
// Invoking generic methods can be done by supplying the correct type arguments
// Dynamic accessor will find the first method that can match this signature
Type value = (Type) accessor.GetSomethingGeneric<int>();
// value == typeof(int)
```

## Accessing hidden value
```cs
class A
{
    string SomeField = "HiddenValue";
}

// B has a field that hides a field in class A
class B : A
{
    string SomeField = string.Empty;
}

// Use the treatAsType parameter to override the type being used
DynamicObjectAccessor accessor = DynamicObjectAccessor.Create(testClass, useCache: true, treatAsType: typeof(A));

var value = (string) accessor.SomeField;
// value == "HiddenValue"
```

## Contributing
Pull requests are welcome, tests will be run on each pull request to ensure integrety of library. 
Make sure a test convers the changes you make.

# License
[GNU GPLv3](https://choosealicense.com/licenses/gpl-3.0/)