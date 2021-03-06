// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using CommandQuerySample.Core.NewCommands;
using CommandQuerySample.Infrastructure.DbContexts;
using CommandQuerySample.Infrastructure.NewCommandHandlers;
using StructureMap;
using StructureMap.Pipeline;
namespace CommandQuerySample.WebUI.DependencyResolution {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.AssembliesFromApplicationBaseDirectory();
                                        scan.WithDefaultConventions();

                                        //Add the assemblies that contains the handlers and validators to the scanning
                                        scan.AssemblyContainingType<NewAddUserCommandHandler>();
                                        scan.IncludeNamespaceContainingType<NewAddUserCommandHandler>();
                                        scan.AssemblyContainingType<NewAddUserCommandValidator>();
                                        scan.IncludeNamespaceContainingType<NewAddUserCommandValidator>();

                                        //Register all types of command validators and handlers
                                        scan.AddAllTypesOf(typeof(ICommandHandler<>));
                                        scan.AddAllTypesOf(typeof(ICommandValidator<>));

                                    });

                            //The context needs to be one instance per http request
                            x.For<ISampleDbContext>().HttpContextScoped().Use<SampleDbContext>();
                        });
            return ObjectFactory.Container;
        }
    }
}