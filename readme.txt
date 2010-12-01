This extension allows that bindings can define that they are a scope for other objects created in the hierarchy below them. All dependencies can now use this object as their scope. Additionally, the extension adds the InCallScope which means one instance for all objects created in one Get cann on the kernel and the InParentScope which mean it has the same lifecycle as the object it is injected into.

InNamedScope
============
Example: You have a ExcelSheet has two components one to draw the sheet and one to update the calculated values on each cell. These two parts of the sheet have a dependency on SheetDataRepository and the same instance shall be used for them. It can't be singleton because you want to be able to create multiple sheets. In this case the bidings can be defined like this:

  const string ScopeName = "ExcelSheet";
  Bind<ExcelSheet>().ToSelf().DefinesNamedScope(ScopeName);
  Bind<SheetPresenter>().ToSelf();
  Bind<SheetCalculator>().ToSelf();
  Bind<SheetDataRepository>().ToSelf().InNamedScope(ScopeName);

In combination with Ninject.Extensions.ContextPreservation this scope type can also be used for bindings that are not created as direct dependency, but sometime later by a factory that was created as dependency. Imagine in the above scenario that the SheetCalculator gets a factory CellCalculatorFacotry injected. Which has a Method CreateCellCalculator that is used by the Sheet calculator to create a cell calculator whenever a formula is added to a cell. The cell calculator needs a SheetDataRepository oc course.

  this.kernel.Load(new NamedScopeModule());
  this.kernel.Load(new ContextPreservationModule());

  const string ScopeName = "ExcelSheet";
  Bind<ExcelSheet>().ToSelf().DefinesNamedScope(ScopeName);
  Bind<SheetPresenter>().ToSelf();
  Bind<SheetCalculator>().ToSelf();
  Bind<CellCalculatorFacotry>().ToSelf();
  Bind<CellCalculator>().ToSelf();
  Bind<SheetDataRepository>().ToSelf().InNamedScope(ScopeName);

NOTE: without ContextPreservationModule you will get an UnknownScopeException.

InCallScope
===========
If this type of scope is added to a binding only one instance is created for one call to kernel.Get<IX>(). In the above scenario the first example can be written as

  Bind<ExcelSheet>().ToSelf();
  Bind<SheetPresenter>().ToSelf();
  Bind<SheetCalculator>().ToSelf();
  Bind<SheetDataRepository>().ToSelf().InCallScope();
  
But the second scenario is not possible because the CreateCellCalculator requests on the factory would produce a new data repository for each call. There is one situation where InCallScope is transparently passed through a Get when Context preservation is used. Imagine that in the above scenario we have two different views onto the data repository using two interfaces. ICellValues and ICellFormulas. In this case the bindings look like this:

  Bind<ExcelSheet>().ToSelf();
  Bind<SheetPresenter>().ToSelf();
  Bind<SheetCalculator>().ToSelf();
  Bind<SheetDataRepository>().ToSelf().InCallScope();
  Bind<ICellValues>().ToMethod(ctx => ctx.ContextPreservingGet<SheetDataRepository>());  
  Bind<ICellFormulas>().ToMethod(ctx => ctx.ContextPreservingGet<SheetDataRepository>());  
  
For the last two bindings the short form can be used
  kernel.BindInterfaceToBinding<ICellValues, SheetDataRepository>();
  kernel.BindInterfaceToBinding<ICellFormulas, SheetDataRepository>();  
  
InParentScope
=============
This is basically the same as InTransientScope() at least when it comes to the lifecycle and the decision which object is injected. As with InTransientScope a new instance is injected for each dependency. The difference to InTransientScope is that bindings with this scope will be deactivated after the object that gets the instnace injected is collected by the garbage collector. E.g. the container disposes the object when it is not used anymore.