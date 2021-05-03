using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.App.Extensions
{
    /// <summary>
    /// Hide the HTML DOM if the User does not have the required Name and Value Claims
    /// </summary>
    [HtmlTargetElement("*", Attributes ="suppress-by-claim-name")]
    [HtmlTargetElement("*", Attributes ="suppress-by-claim-value")]
    public class HideElementByClaimTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HideElementByClaimTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HtmlAttributeName("suppress-by-claim-name")]
        public string IdentityClaimName { get; set; }
        
        [HtmlAttributeName("suppress-by-claim-value")]
        public string IdentityClaimValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) 
                throw new ArgumentNullException(nameof(context));
            if (output == null) 
                throw new ArgumentNullException(nameof(context));

            var hasAccess = CustomAuthorization.ValidarClaimsUsuario(_contextAccessor.HttpContext, IdentityClaimName, IdentityClaimValue);

            if (hasAccess) return;

            output.SuppressOutput();
        }
    }

    /// <summary>
    /// Disable the HTML DOM if the User does not have the required Name and Value Claims
    /// </summary>
    [HtmlTargetElement("a", Attributes="disable-by-claim-name")]
    [HtmlTargetElement("a", Attributes="disable-by-claim-value")]
    public class DisableHrefByClaimTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public DisableHrefByClaimTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HtmlAttributeName("suppress-by-claim-name")]
        public string IdentityClaimName { get; set; }

        [HtmlAttributeName("suppress-by-claim-value")]
        public string IdentityClaimValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(context));

            var hasAccess = CustomAuthorization.ValidarClaimsUsuario(_contextAccessor.HttpContext, IdentityClaimName, IdentityClaimValue);

            if (hasAccess) return;

            output.Attributes.RemoveAll("href");
            output.Attributes.Add("href", "");
            
            output.Attributes.RemoveAll("title");
            output.Attributes.Add("title", "Usuário sem permissão");
          
            output.Attributes.RemoveAll("placement");
            output.Attributes.Add("placement", "bottom");

            output.Attributes.Add("style", "cursor: not-allowed");
        }
    }

    /// <summary>
    /// Show the HTML DOM just if the User is at a specific Action 
    /// </summary>
    [HtmlTargetElement("*", Attributes = "show-by-action")]
    public class ShowElementByActionTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ShowElementByActionTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HtmlAttributeName("show-by-action")]
        public string ActionName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(context));

            var currentAction = _contextAccessor.HttpContext.GetRouteData().Values["action"].ToString();

            if (ActionName.Contains(currentAction)) return;

            output.SuppressOutput();
        }
    }
}
