﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Seznam.Data;
using Seznam.Data.Services.List;
using Seznam.Data.Services.List.Contracts;
using Seznam.Data.Services.User;
using Seznam.Data.Services.User.Contracts;
using Seznam.Models;
using Seznam.Utilities;
using System.Web.Helpers;
using SeznamList = Seznam.Data.Services.List.Contracts.SeznamList;
using SeznamListItem = Seznam.Data.Services.List.Contracts.SeznamListItem;
using User = Seznam.Models.User;

namespace Seznam.Controllers
{
    public class ListController : Controller
    {
        private readonly IListService _listService;
        private readonly ISessionContext _sessionContext;
        private ILogger _logger;

        public ListController()
        {
            _logger = new NullLogger();
            _listService = new ListService();
            _sessionContext = SessionContext.Current;
        }

        public ListController(IListService listService, ISessionContext sessionContext, ILogger logger)
        {
            _listService = listService;
            _sessionContext = sessionContext;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Index2()
        {
            return View();
        }

        [HttpGet]
        public ViewResult My()
        {
            return View("List");
        }

        [HttpGet]
        public ViewResult Shared()
        {
            return View();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonNetResult All()
        {
            var user = _listService.GetSummary(_sessionContext.UserId, _sessionContext.Username);
            
            return DataResponse.Success(user);
        }


        [HttpPut]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonNetResult Create(NewList data)
        {
            var list = _listService.CreateList(new SeznamList(_sessionContext.UserId, data.Name, data.Shared, data.Users));

            return list.ToJsonResult();
        }

        [HttpPut]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonNetResult CreatePersonalListItem(NewListItem item)
        {
            try
            {
                var i = _listService.CreateListItem(item.ListId, item.Name, item.Count);
                return DataResponse.Success(i);
            }
            catch (ListItemExistsException ex)
            {
                _logger.Error(ex);
                return DataResponse.Error(ex.Message);
            }
        }

        [HttpDelete]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonNetResult DeletePersonalListItem(DeletePersonalListItemMessage message)
        {
            _listService.DeleteItem(message.ListId, message.Name);

            return SimpleResponse.Success();
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonNetResult UpdatePersonalListItem(NewListItem data)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonNetResult TogglePersonalListItem(ToggleData data)
        {
            var item = _listService.TogglePersonalListItem(data.ListId, data.ItemName, data.ItemCompleted);

            return DataResponse.Success(item);
        }
    }
}
