using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using notebook.Controllers.Resources;
using notebook.DataLayer.Abstracts;
using notebook.Helpers;
using notebook.Models;

namespace notebook.Controllers
{
  public class NoteController : Controller
  {
    private readonly INoteService _noteService;
    private readonly IMapper _mapper;

    public NoteController(INoteService noteService, IMapper mapper)
    {
      _mapper = mapper;
      _noteService = noteService;
    }

    [HttpGet]
    [Route("api/notes/{userId}")]
    public async Task<IEnumerable<Note>> GetTask(string userId)
    {
      return await _noteService.GetAllNotes(userId);
    }

    [HttpGet]
    [Authorize]
    [Route("api/note/{id}")]
    public async Task<IActionResult> GetOne(string id)
    {
      try
      {
        var identity = User.Identity as ClaimsIdentity;
        var userId = identity.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        var note = await _noteService.GetNote(id , userId);
        if (note == null)
        {
          return BadRequest("Note not found!");
        }
        return Ok(note);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.ToString());
      }
    }

    [HttpPost]
    [Authorize]
    [Route("api/note")]
    public async Task<IActionResult> CreateNote([FromBody]CreateNoteResource model)
    {
      try
      {
        var note = _mapper.Map<CreateNoteResource, Note>(model);
        note.UserId = ObjectId.Parse(model.UserId);
        note.Category.Id = ObjectId.Parse(model.CategoryId);
        if (string.IsNullOrWhiteSpace(note.Title))
          return BadRequest("Title not allowed!");
        note.Create = DateTime.UtcNow;
        var resutl = await _noteService.AddNote(note);
        if (resutl)
            return Ok("Note added.");
        return BadRequest("Note coudn't add!");
      }
      catch (Exception ex)
      {
        return BadRequest(ex.ToString());
      }
    }

    [HttpPut]
    [Authorize]
    [Route("api/note/update/{id}")]
    public async Task<IActionResult> UpdateNote([FromBody]CreateNoteResource model, string id)
    {
      var identity = User.Identity as ClaimsIdentity;
      var userId = identity.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
      var note = _mapper.Map<CreateNoteResource, Note>(model);
      note.Category.Id = ObjectId.Parse(model.CategoryId);
      if (string.IsNullOrWhiteSpace(note.Title))
        return BadRequest("Title is missing");
      note.Update = DateTime.UtcNow;
      var result = await _noteService.UpdateNote(note, userId, id);
      if (result)
      {
        return Ok("Note updated successfully.");
      }
      return BadRequest("Note not found!");
    }

    [HttpDelete]
    [Authorize]
    [Route("api/note/{id}")]
    public async Task<IActionResult> DeleteNote(string id)
    {
      try
      {
        var identity = User.Identity as ClaimsIdentity;
        var userId = identity.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrWhiteSpace(id))
          return BadRequest("Note title missing");
        await _noteService.RemoveNote(id, userId);
        return Ok("Note has been deleted.");
      }
      catch (Exception ex)
      {
        return BadRequest(ex.ToString());
      }
    }

    [Authorize]
    [HttpDelete]
    [Route("api/note/deleteAll")]
    public async Task<IActionResult> DeleteAllNotes()
    {
      try
      {
        var identity = User.Identity as ClaimsIdentity;
        var userId = identity.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        await _noteService.RemoveAllNotes(userId);
        return Ok("All notes deleted successfully.");
      }
      catch (Exception ex)
      {
        return BadRequest(ex.ToString());
      }
    }
  }
}