using androkat.maui.library.Abstraction;
using androkat.maui.library.Models.Entities;

namespace androkat.maui.library.ViewModels;

public partial class GyonasMirrorViewModel : ViewModelBase
{
    private readonly IPageService _pageService;

    public GyonasMirrorViewModel(IPageService pageService)
    {
        _pageService = pageService;
    }

    public List<Bunok> BunokList { get; set; } = [];

    public async Task InitializeAsync()
    {
        await FetchAsync();
    }

    public async Task<int> InsertBunok(Bunok entity)
    {
        return await _pageService.InsertBunok(entity);
    }

    public async Task<int> DeleteBunokByIds(int bunId, int parancsId)
    {
        return await _pageService.DeleteBunokByIds(bunId, parancsId);
    }

    async Task FetchAsync()
    {
        BunokList = await _pageService.GetBunok();
    }
}