﻿using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DogGo.Controllers
{
    public class DogsController : Controller
    {
        private readonly IDogRepository _dogRepo;

        public DogsController(IDogRepository dogRepository)
        {
            _dogRepo = dogRepository;
        }






        // GET: DogsController
        [Authorize]
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();

            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(ownerId);

            return View(dogs);
        }

        // GET: DogsController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            List<Dog> dog = _dogRepo.GetDogsByOwnerId(id);

            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }






        // GET: DogsController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                // update the dogs OwnerId to the current user's Id
                dog.OwnerId = GetCurrentUserId();

                _dogRepo.AddDog(dog);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }






        // GET: DogsController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            int ownerId = GetCurrentUserId();


            if (dog == null )
            {
                return NotFound();
            }
            else
            {
                if (dog.OwnerId == ownerId)
                {
                    return View(dog);
                }
                else
                {
                    return NotFound();
                }
            }

        }

        // POST: DogsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                _dogRepo.UpdateDog(dog);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }






        // GET: DogsController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            int ownerId = GetCurrentUserId();

            if (dog.OwnerId == ownerId)
            {
                return View(dog);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: DogsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                _dogRepo.DeleteDog(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }






        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
