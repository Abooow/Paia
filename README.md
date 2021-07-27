# Paia
Paia is a CLI framework that enables you to easily render and change between different views in a Console application.

</br>

***NOTE:** This is a **very** early version, a lot of code breaking changes will occur in later versions*
___

**Table of Contents**
- [Usage](#usage)
  - [Installing the library](#installing-the-library)
- [Initialize the App](#initialize-the-app)
- [Create, Render and Change View](#create-render-and-change-view)
  - [Creating a View](#creating-a-view)
  - [Change between Views](#change-between-views)
- [Dependency Injection](#dependency-injection)
  - [Adding Dependency Injection](#adding-dependency-injection)
  - [Injecting services](#injecting-services)
- [More](#more)
  - [Exit() and exitcode](#exit-and-exitcode)

___

## Usage
See the [sample project](./tests/Paia.CliTest/) for a basic usage of the framework.

### Installing the library
This project is not available as a NuGet package yet :(
```sh
PM> Install-Package ...
```
*-- or --*  
Clone the repository
```sh
# if you are using ssh
$ git clone git@github.com:Abooow/Paia.git

# if you are using https
$ git clone https://github.com/Abooow/Paia.git
```

</br>

## Initialize the App
```csharp
using Paia;

static void Main(string[] args)
{
  new AppBuilder()
    .Build()
    .Run<MyFirstView>();
}
```

</br>

## Create, Render and Change View
### Creating a View
To create a View you have to inherit the `ViewBase` class (implementing the IView interface also works fine) and then override the **Render** method.
```csharp
using Paia.Views;

class MyFirstView : ViewBase
{
  public override ViewResult Render()
  {
    Console.Clear();
    
    Console.WriteLine("Hello World!");
    Console.WriteLine();
    Console.WriteLine("Press 1 to exit");
    char input = Console.ReadKey().KeyChar;
    
    return input switch
    {
      '1' => Exit(),
       _  => ReRenderView()
    };
  }
}
```
The **Render** method returns a `ViewResult`, which basically holds information about what to do next after this View finishes rendering. </br>
`Exit()` will return a ViewResult telling the framework to exit the application with exitcode 0. (Read more about Exit() and exitcode [here](#exit-and-exitcode)) </br>
`ReRenderView()` tells the framework to stay and re-render the same View. The state of the view will **NOT** change, the App will keep the same instance and only call the Render method again. If you want to re-render the same View but not keep the state, use the `Refresh()` method instead... which doesn't exist yet :)

### Change between Views
To change view, simply call the `ChangeView<TView>()`.
```csharp
using Paia.Views;

class MyFirstView : ViewBase
{
  public override ViewResult Render()
  {
    Console.Clear();
    
    Console.WriteLine("Hello World!");
    Console.WriteLine();
    Console.WriteLine("Press 1 to exit, 2 to change View");
    char input = Console.ReadKey().KeyChar;
    
    return input switch
    {
      '1' => Exit(),
      '2' => ChangeView<MySecondView>(),
       _  => ReRenderView()
    };
  }
}

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~

class MySecondView : ViewBase
{
  public override ViewResult Render()
  {
    Console.Clear();
    Console.WriteLine("Hello World from my Second View!");
    Console.ReadKey();
    
    return GoBack();
  }
}
```
When changing to a new View, the old View (MyFirstView) will be pushed to a stack (BackStack) by the ViewManager, making it possible to return to it if needed. The `GoBack()` method pops the stack and renders the previous View (MyFirstView). The current View (MySecondView) will pushed to yet another stack, the FrontStack, so it is possible to go forward as well by calling the `GoForward()` method.

</br>

## Dependency Injection
### Adding Dependency Injection
Add services to the app by calling the `.ConfigureServices(Action<IServiceCollection>)` method on AppBuilder in **Program.cs**
```csharp
static void Main(string[] args)
{
  new AppBuilder()
    .ConfigureServices(services => services.AddSingleton<IService, IImplementation>())
    .Build()
    .Run<MyFirstView>();
}
```

### Injecting services
To inject a service to a view, use the `[Inject]` attribute on a public property with a setter.
```csharp
using Paia;
using Paia.Attributes;

class MyFirstView : ViewBase
{
  [Inject]
  public IService MyService { get; set; }

  public override ViewResult Render()
  {
    ...
  }
}
```

</br>

## More
### Exit() and exitcode
To exit the application with a exitcode other than 0, you have to change the `Main` method to the following:
```csharp
using Paia;

static int Main(string[] args)
{
  return new AppBuilder()
      .Build()
      .Run<MyFirstView>();
}
```
The `Run()` method returns an int, which is the exit code for the application. </br>
`Exit()` (called from Views) will return 0 by default, but has a overload allowing you to pass an integer as a argument for a custom exitcode. </br>
</br>
```csharp
using Paia;

class MyFirstView : ViewBase
{
  public override ViewResult Render()
  {
    return 69;
  }
}
```
It's possible to also return an integer directly from a View, there is an implicit conversion from int to ViewResult. This is telling the View to Exit with exitcode 69.
