This extension allows that bindings can define scopes. All dependencies can now define that they want to use this 
instance as their scope.

E.g. An object A has two dependencies B1 and B2. B1 and B2 themselves have a dependency C. Both want to use the same 
instance. In this case the bidings can be defined like this:
  const string ScopeName = "SomeUniqueScpoeName";
  Bind<A>().ToSelf().DefinesNamedScope(ScopeName);
  Bind<B1>().ToSelf();
  Bind<B2>().ToSelf();
  Bind<C>().ToSelf().InNamedScope(ScopeName);

In combination with Ninject.Extensions.ContextPreservation this scope type can also be used for bindings that are
not created as dependency, but sometime later by a factory that was created as dependency. Imagine in the above scenario
that A gets also a factory F injected. And that A has a method Foo, that calls CreateB2 on that factory. the bindings
would be the following in this case:

  this.kernel.Load(new NamedScopeModule());
  this.kernel.Load(new ContextPreservationModule());

  const string ScopeName = "SomeUniqueScpoeName";
  this.kernel.Bind<A>().ToSelf().DefinesNamedScope(ScopeName);
  this.kernel.Bind<B1>().ToSelf();
  this.kernel.Bind<B2>().ToSelf();
  this.kernel.Bind<F>().ToSelf();
  this.kernel.Bind<C>().ToSelf().InNamedScope(ScopeName);

It is possible now to call Foo on a new instance of A and it will still get the same instance of C.
  var a = this.kernel.Get<A>();
  a.Foo();

NOTE: without ContextPreservationModule yu will get an UnknownScopeException.


