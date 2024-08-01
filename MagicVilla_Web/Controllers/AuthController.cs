﻿using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_Web.Controllers;

public class AuthController : Controller
{
    #region DI

    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    #endregion

    #region LOGIN

    [HttpGet]
    public IActionResult Login()
    {
        LoginRequestDTO obj = new();
        return View(obj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequestDTO obj)
    {
        return View();
    }

    #endregion

    #region REGISTER

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterationRequestDTO obj)
    {
        APIResponse result = await _authService.RegisterAsync<APIResponse>(obj);
        if (result != null && result.IsSuccess)
        {
            return RedirectToAction("Login");
        }

        return View();
    }

    #endregion

    public async Task<IActionResult> Logout()
    {
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}