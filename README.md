# csvdotnet

A simple, fluent, CSV writer for dotnet core 3.1
[![Coverage Status](https://coveralls.io/repos/github/jwrob/csvdotnet/badge.svg?branch=)](https://coveralls.io/github/jwrob/csvdotnet?branch=)

# Usage

```
var someList = new List<string> { "foo", "bar", "bas" };

someList.Csv()
  .Field("head1", entry => entry)
  .ToString()
```
