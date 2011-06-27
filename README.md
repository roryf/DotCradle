DotCradle
======

A thing wrapper round CouchDb's REST API for .NET apps.

Introduction
------------

Heavily inspired by the node.js [cradle](http://cloudhead.io/cradle) library.

Not much to see yet, just an experiment in progress. The intention is to provide a simple API to that other libraries don't have to worry about any HTTP gubbins when talking to CouchDb, they simply call `Get`, `Post` etc. It also does not intend to provide any method of JSON serialization/deserialization, that is left to the consuming library (I also intend to write a wrapper somewhat similar to [Simple.Data](https://github.com/markrendle/Simple.Data) that works with `dynamic` objects and takes care of JSON etc.).

Plan
----

A very rough list of stuff:

1. test data auto-setup
1. get list of dbs
2. create db
3. get list of docs
4. add doc
5. retrieve doc
6. update doc
7. delete doc
8. update multiple docs
11. attachments
12. rake build file
13. NuGet package