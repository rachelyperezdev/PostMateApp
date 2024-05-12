namespace PostMateApp.Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, ViewModel, Model> 
        where SaveViewModel : class
        where ViewModel : class
        where Model : class
    {
        Task<SaveViewModel> Add(SaveViewModel vm);
        Task Update(SaveViewModel vm, int id);
        Task Delete(int id);
        Task<SaveViewModel> GetByIdSaveViewModel(int id);
        Task<List<ViewModel>> GetAllViewModel();
    }
}
