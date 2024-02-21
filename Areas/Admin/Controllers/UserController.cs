﻿using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.Constants;
using QFX.data;
using QFX.Entity;
using QFX.ViewModels.UserVm;

namespace QFX.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notifyService;

    public UserController(ApplicationDbContext context, INotyfService notyfService)
    {
        _context = context;
        _notifyService = notyfService;
    }
    public IActionResult Index()
    {
        var vm = new IndexVm();
        vm.Users = _context.Users.Include(x=>x.Location).ToList();
        
        return View(vm);
    }
    public IActionResult Add()
    {
        var vm = new UpsertVm();
        vm.Location = _context.Locations.ToList();
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Add(UpsertVm vm)
    {
        try
        {   using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var user = new User();
            user.Name = vm.Name;
            user.Email = vm.Email;
            user.PhoneNo = vm.PhoneNo;
            user.PasswordHash = vm.Password;
            user.LocationID = vm.LocationID;
            user.UserType = UserTypeConstants.Employee;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            tx.Complete();
            _notifyService.Success("user added.!!!");
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notifyService.Error("error operation" + e.Message);
            return RedirectToAction("Index");
        }
        
    }
    public IActionResult Update(long ID)
    {
        var vm = new UpsertVm();
        var user = _context.Users.FirstOrDefault(x => x.ID == ID);

        vm.Name = user.Name;
        vm.PhoneNo = user.PhoneNo;
        vm.Email = user.Email;
        vm.UserStatus = user.UserStatus;
        vm.UserType = user.UserType;
        vm.DateOfBirth = user.DateOfBirth;
        vm.LocationID = user.LocationID;
        vm.Location = _context.Locations.ToList();
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(long ID, UpsertVm vm)
    {
        try
        {   using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.ID==ID);
            user.Name = vm.Name;
            user.PhoneNo = vm.PhoneNo;
            user.Email = vm.Email;
            user.UserStatus = vm.UserStatus;
            user.DateOfBirth = vm.DateOfBirth;
            user.LocationID = vm.LocationID;
            user.UserType = vm.UserType;
            await _context.SaveChangesAsync();
            tx.Complete();
            _notifyService.Success("User updated!!!");
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _notifyService.Error("Operation fail" + e.Message);
            return RedirectToAction("Index");
        }
    }
    public IActionResult Delete(long ID)
    {
        return RedirectToAction("Index");
    }
}
