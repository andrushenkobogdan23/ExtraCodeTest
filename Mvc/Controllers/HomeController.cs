using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoServices.Shared.Dto;
using MVC.Models.Home;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Shared.HttpClient;
using Shared.Enum;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization;
using TodoServices.Domain;
using TodoServices.Domain.Model;

namespace MVC.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        
        public HomeController(IStringLocalizer<HomeController> localizer) : base()
        {
            _localizer = localizer;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var model = await FetсhIndexModel();

            // Retrieves the requested culture
            var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            // Culture contains the information of the requested culture
            var culture = rqf.RequestCulture.Culture;
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost("")]
        public async Task<IActionResult> Index(BaseFilter filter)
        {
            filter.OnlyParent = true;
            var model = await FetсhIndexModel(filter);

            return View(model);
        }

        [HttpGet("Create")]
        public IActionResult Create(int? parentId = null)
        {
            return View(new AddTodoCommand { ParentId = parentId });
        }

        [ValidateAntiForgeryToken]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(AddTodoCommand cmd)
        {
           
            if (ModelState.IsValid)
            {
                cmd.AuthorId = UserId;

                var res = await HttpRequestFactory.Post(APIUrls.TodosCmd, cmd, BearerToken, APIUrls.TodosCmd_V10);

                if (res.IsSuccessful)
                    return RedirectToAction(nameof(Index));

                AddResponseErrorsToModelState(res);
            }

            return View(cmd);
        }

        
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resItem = await HttpRequestFactory.Get($"{APIUrls.Todos}/{id}", BearerToken, APIUrls.Todos_V10);

            if (!resItem.IsSuccessful)
                return RedirectToAction(nameof(Index));

            var todo = resItem.Response.ContentAsType<TodoInfo>();

            return View(todo);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, TodoInfo model)
        {
            var url = $"{APIUrls.TodosCmd}/?id={id}&userid={UserId}";
            var res = await HttpRequestFactory.Delete(url, BearerToken, APIUrls.TodosCmd_V11);

            if (res.IsSuccessful)
            {
                return RedirectToAction(nameof(Index));
            }

            AddResponseErrorsToModelState(res);
          
            return View(model);
        }


        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TodoInfo model)
        {
            if (ModelState.IsValid)
            {
                var cmd = new UpdateTodoCommand {
                    Title = model.Title,
                    Priority = model.Priority,
                    EstimatedCost = model.EstimatedCost,
                    Description = model.Description,
                    AuthorId = UserId,
                    Id = model.Id,
                    ParentId = model.ParentId
                };

                var uri = $"{APIUrls.TodosCmd}/{id}";
                var res = await HttpRequestFactory.Put(uri, cmd, BearerToken, APIUrls.TodosCmd_V10);

                if(res.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                else
                    AddResponseErrorsToModelState(res);
            }

            return View(model);

        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var resItem = await HttpRequestFactory.Get($"{APIUrls.Todos}/{id}", BearerToken, APIUrls.Todos_V10);

            if (!resItem.IsSuccessful)
                return RedirectToAction(nameof(Index));

            var todo = resItem.Response.ContentAsType<TodoInfo>();
            return View(todo);
        }

        [HttpGet("Finish/{id}")]
        public async Task<IActionResult> Finish(int id)
        {
            var resItem = await HttpRequestFactory.Get($"{APIUrls.Todos}/{id}", BearerToken, APIUrls.Todos_V10);
            //string errorMassege = "You must Finish children Task";

            if (!resItem.IsSuccessful)
                return RedirectToAction(nameof(Index));

            var todo = resItem.Response.ContentAsType<TodoInfo>();
            return View(todo);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            
            var resItem = await HttpRequestFactory.Get($"{APIUrls.Todos}/{id}", BearerToken, APIUrls.Todos_V10);
           // string errorMassege = "You must Finish children Task";

            if (!resItem.IsSuccessful)
                return RedirectToAction(nameof(Index));


            var todo = resItem.Response.ContentAsType<TodoInfo>();          
            return View(todo);
            //var todo = await _work.LoadTodoAsync(id);
            //return View(todo);
        }

        [HttpPost("Finish/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finish(int id, FinishModel model)
        {
            if (ModelState.IsValid)
            {
                var cmd = new CompleteTodoCommand(model.Id, UserId, model.Cost);
                var apiUri = $"{APIUrls.TodosCmd}/finish/{model.Id}";

                var res = await HttpRequestFactory.Put(apiUri, cmd, BearerToken, APIUrls.TodosCmd_V10);
                //var res = await _work.UpdateTodoAsync(cmd);

                if (res.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                else
                    AddResponseErrorsToModelState(res);
            }


            TodoInfo todo;
            var resItem = await HttpRequestFactory.Get($"{APIUrls.Todos}/{id}", BearerToken, APIUrls.Todos_V10);
            if(!resItem.IsSuccessful)
                return RedirectToAction(nameof(Index));

            todo = resItem.Response.ContentAsType<TodoInfo>();
            todo.Cost = model.Cost;
            return View(todo);
        }

        private async Task<IndexModel> FetсhIndexModel(BaseFilter filter = null)
        {
            //todo: we could use IndexDb to restore whole IndexModel or separetely: filter, items, users
            filter = filter ?? new BaseFilter { OnlyParent = true}; 
            var users = LoadUsers(UserId, User.Claims.Where(s => s.Type == "name").FirstOrDefault().Value);

            var model = new IndexModel
            {
                From = filter.From,
                To = filter.To,
                PerformerId = filter.PerformerId,
                Users = users.Select(s =>
                    new SelectListItem
                    {
                        Text = s.Value,
                        Value = s.Key.ToString(),
                        Selected = filter.PerformerId == s.Key,
                        Disabled = s.Key == 1
                    }),
            };




            var response = await HttpRequestFactory.Post(APIUrls.Todos, filter, BearerToken, APIUrls.Todos_V10);

            if (response.IsSuccessful)
                model.Items = response.Response.ContentAsType<IEnumerable<TodoInfo>>();
            else
                AddResponseErrorsToModelState(response);

            return model;
        }





        private async Task<TodoInfo> FetсhTodoInfoModel(BaseFilter filter = null)
        {
            //todo: we could use IndexDb to restore whole IndexModel or separetely: filter, items, users
            filter = filter ?? new BaseFilter();
            var users = LoadUsers(UserId, User.Claims.Where(s => s.Type == "name").FirstOrDefault().Value);

            var model = new TodoInfo
            {
                CreateDate = filter.From,
                CompleteDate = filter.To,
                PerformerId = filter.PerformerId,              
            };
            var response = await HttpRequestFactory.Post(APIUrls.Todos, filter, BearerToken, APIUrls.Todos_V10);

            if (response.IsSuccessful)
                model.Childs = response.Response.ContentAsType<List<TodoInfo>>();
            else
                AddResponseErrorsToModelState(response);

            return model;
        }



        internal IEnumerable<KeyValuePair<int, string>> LoadUsers(int id, string name)
        {
            return new List<KeyValuePair<int, string>>() {
                new KeyValuePair<int, string> (-1, _localizer["All todos"]),
                new KeyValuePair<int, string> (0, _localizer["Not completed todos"]),
                new KeyValuePair<int, string> (id, name),
                new KeyValuePair<int, string> (200, "User 200"),
                new KeyValuePair<int, string> (300, "User 300"),
            };
        }
    }
}