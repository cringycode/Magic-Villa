﻿using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaNumberController : Controller
{
    #region DI

    private readonly IVillaNumberService _villaNumberService;
    private readonly IVillaService _villaService;
    private readonly IMapper _mapper;

    public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService)
    {
        _villaNumberService = villaNumberService;
        _villaService = villaService;
        _mapper = mapper;
    }

    #endregion

    #region INDEX

    public async Task<IActionResult> IndexVillaNumber()
    {
        List<VillaNumberDTO> list = new();

        var response = await _villaNumberService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
        }

        return View(list);
    }

    #endregion

    #region CREATE

    public async Task<IActionResult> CreateVillaNumber()
    {
        VillaNumberCreateVM villaNumberVM = new();
        var response = await _villaService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                (Convert.ToString(response.Result)).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        return View(villaNumberVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.CreateAsync<APIResponse>(model.VillaNumber);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
        }

        return View(model);
    }

    #endregion

    // #region UPDATE
    //
    // public async Task<IActionResult> UpdateVilla(int villaId)
    // {
    //     var response = await _villaNumberService.GetAsync<APIResponse>(villaId);
    //     if (response != null && response.IsSuccess)
    //     {
    //         VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
    //         return View(_mapper.Map<VillaUpdateDTO>(model));
    //     }
    //
    //     return NotFound();
    // }
    //
    // [HttpPost] // i think it should PUT.
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         var response = await _villaNumberService.UpdateAsync<APIResponse>(model);
    //         if (response != null && response.IsSuccess)
    //         {
    //             return RedirectToAction(nameof(IndexVillaNumber));
    //         }
    //     }
    //
    //     return View(model);
    // }
    //
    // #endregion

    #region DELETE

    public async Task<IActionResult> DeleteVilla(int villaId)
    {
        var response = await _villaNumberService.GetAsync<APIResponse>(villaId);
        if (response != null && response.IsSuccess)
        {
            VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
            return View(model);
        }

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVilla(VillaDTO model)
    {
        var response = await _villaNumberService.DeleteAsync<APIResponse>(model.Id);
        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(IndexVillaNumber));
        }

        return View(model);
    }

    #endregion
}