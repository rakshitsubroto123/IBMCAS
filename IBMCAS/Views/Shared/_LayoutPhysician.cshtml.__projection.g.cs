//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP {
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using System.Web.UI;
using System.Web.WebPages;
using System.Web.WebPages.Html;

public class _Page__LayoutPhysician_cshtml : System.Web.WebPages.WebPage {
private static object @__o;
#line hidden
public _Page__LayoutPhysician_cshtml() {
}
protected System.Web.HttpApplication ApplicationInstance {
get {
return ((System.Web.HttpApplication)(Context.ApplicationInstance));
}
}
public override void Execute() {

#line 1 "C:\Users\RishiAgarwal\AppData\Local\Temp\1CF6578486F76383937B91AB96C4C09CA65B\2\IBMCAS\IBMCAS\Views\Shared\_LayoutPhysician.cshtml"
  

    Layout = "~/Views/Shared/_Layout.cshtml";


#line default
#line hidden
DefineSection("NavBarMenu", () => {


#line 2 "C:\Users\RishiAgarwal\AppData\Local\Temp\1CF6578486F76383937B91AB96C4C09CA65B\2\IBMCAS\IBMCAS\Views\Shared\_LayoutPhysician.cshtml"
                                                          __o = Url.Action("AddNewPhysician","Admin");


#line default
#line hidden
});


#line 3 "C:\Users\RishiAgarwal\AppData\Local\Temp\1CF6578486F76383937B91AB96C4C09CA65B\2\IBMCAS\IBMCAS\Views\Shared\_LayoutPhysician.cshtml"
__o = RenderBody();


#line default
#line hidden
}
}
}