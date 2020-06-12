# EFCore.Plugable

***This Project is work in Progress!***

The goal of this Library is to add Plugin functionality to EntityFrameworkCore.

## Feature List:

- [x] semi-dynamic Model
- [x] migrations
- [o] `dotnet-ef` Code Generation

And probably more...

## Working with Migrations

`dotnet ef migrations add --project efcore.plugable.test InitialCreation --verbose -o test/simple.plugin/migrations`

Creates the initial Migration, has anybody an Idea how to test this?