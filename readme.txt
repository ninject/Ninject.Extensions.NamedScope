This Ninject Extension enables more powerful higher-level scoping mechanisms to be defined than those offered in the Kernel itself such as
- InParentScope to define a lifetime relative to that of the Request that triggered a resolution of a Binding
- InCallScope to tie the lifetime to that of the Root Request [and constrain to a single instance within the hierarchy of the] Request being Resolved (See also Ninject.Extensions.ContextPreservation for the ability to be able to extend this to items generated via factories (including Ninject.Extensions.Factory) later in the processing lifecycle)
- DefinesNameScope / InNamedScope to define a custom pooling rules which one can explicitly Kernel.Release as part of the processing flow 

More information is found in the wiki: https://github.com/ninject/ninject.extensions.namedscope/wiki
