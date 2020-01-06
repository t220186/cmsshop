using Cms.Models.Data;
using System.ComponentModel.DataAnnotations;

namespace Cms.Models.ViewModels.Pages
{
    public class SideBarViewModel
    {
        public SideBarViewModel() { }
        public SideBarViewModel(SideBarDTO row)
        {
            Id = row.Id;
            Body = row.Body;
        }

        public int Id { get; set; }
        [Required]
        [StringLength(int.MaxValue,MinimumLength =1)]
        public string Body { get; set; }
    }

}