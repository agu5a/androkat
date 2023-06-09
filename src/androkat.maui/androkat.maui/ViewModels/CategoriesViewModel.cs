using androkat.hu.Models;
using androkat.hu.Pages;
using androkat.maui.library.Models;
using androkat.maui.library.Models.Responses;
using androkat.maui.library.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace androkat.hu.ViewModels;

public partial class CategoriesViewModel : ViewModelBase
{
    private readonly PageService showsService;

    [ObservableProperty]
    List<Category> categories;

    public CategoriesViewModel(PageService shows)
    {
        showsService = shows;
    }

    public async Task InitializeAsync()
    {
        //var categories = await showsService.GetAllCategories();

        Categories = new List<Category> { new Category(new CategoryResponse { }) { } };//categories?.ToList();
    }

    [RelayCommand]
    Task Selected(Category category)
    {
        return Shell.Current.GoToAsync($"{nameof(CategoryPage)}?Id={category.Id}");
    }
}
