# csvdotnet

A simple, fluent, CSV writer for dotnet core 3.1  
[![Coverage Status](https://coveralls.io/repos/github/jwrob/csvdotnet/badge.svg?branch=)](https://coveralls.io/github/jwrob/csvdotnet?branch=)  
[![codecov](https://codecov.io/gh/jwrob/csvdotnet/branch/primary/graph/badge.svg)](https://codecov.io/gh/jwrob/csvdotnet)  

# Usage

## Basic Example

```
class User
{
    public string First { get; }
    public string Last { get; }

    public User(string first, string last) =>
        (First, Last) = 
        (first, last);
}

var someList = new List<User> { new User("Jane", "Doe"), new User("Bob", "White") };

var csvContents = someList.Csv()
                    .Field("FirstName", entry => entry.First)
                    .Field("LastName", entry => entry.Last)
                    .ToString()

```

## Features

Use the `.EndingCrLf()` method to include a CrLf at the end of the file. Either way is permissible by the spec.

Use the `.ExcludeHeader()` method to remove the header from the beginning of the file. Either way is permissible by the spec.

# References

[RFC4180](https://tools.ietf.org/html/rfc4180)
