# Ninject.Extensions.Factory

[![Build status](https://ci.appveyor.com/api/projects/status/lg2gqoo5drnk1d5e?svg=true)](https://ci.appveyor.com/project/Ninject/ninject-extensions-namedscope)
[![codecov](https://codecov.io/gh/ninject/Ninject.Extensions.NamedScope/branch/master/graph/badge.svg)](https://codecov.io/gh/ninject/Ninject.Extensions.NamedScope)
[![NuGet Version](http://img.shields.io/nuget/v/Ninject.Extensions.NamedScope.svg?style=flat)](https://www.nuget.org/packages/Ninject.Extensions.NamedScope/) 
[![NuGet Downloads](http://img.shields.io/nuget/dt/Ninject.Extensions.NamedScope.svg?style=flat)](https://www.nuget.org/packages/Ninject.Extensions.NamedScope/)

This Ninject Extension enables more powerful higher-level scoping mechanisms to be defined than those offered in the Kernel itself such as:
- `InParentScope` to define a lifetime relative to that of the `Request` that triggered a resolution of a `Binding`
- `InCallScope` to tie the lifetime to that of the Root Request [and constrain to a single instance within the hierarchy of the Request] being Resolved
- `DefinesNamedScope` / `InNamedScope` to enable custom pooling rules
- `CreateNamedScope` allows a Named Scope equivalent to one induced by `DefinesNamedScope()` to be generated programmatically (and Disposed as desired)

More information is found in the wiki: https://github.com/ninject/ninject.extensions.namedscope/wiki