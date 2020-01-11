using Cms.Models.Data;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Cms.Models.ViewModels.Pages
{


    public class PageViewModel
    {
        public PageViewModel()
        { }
        public PageViewModel(PageDTO row)
        {
            Id = row.Id;
            Title = row.Title;
            Slug = row.Slug;
            Body = row.Body;
            Sorting = row.Sorting;
            HasSidebar = row.HasSidebar;

        }

        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength = 3)]
        [Display(Name = "Tytuł strony")]
        public string Title { get; set; }
        [Display(Name = "Adres (indywidualny) strony")]
        public string Slug { get; set; }
        [Required]
        [AllowHtml]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        [Display(Name = "Treść strony")]
        public string Body { get; set; }
        [Display(Name = "Sortowanie")]
        public int Sorting { get; set; }
        [Display(Name = "Pasek boczny")]
        public bool HasSidebar { get; set; }
    }
}