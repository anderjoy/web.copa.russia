using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWebAPI.TagHelpers
{
    public class ImageTagHelper: TagHelper
    {
       public byte[] File { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";

            output.Attributes.SetAttribute("src", $"data:image/png;base64,{Convert.ToBase64String(File)}");
        }
    }
}
