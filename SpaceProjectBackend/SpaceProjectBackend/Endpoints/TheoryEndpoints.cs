using Microsoft.AspNetCore.Mvc;
using SpaceProjectBackend.Repository;
using SpaceProjectBackend.Models;
using Microsoft.AspNetCore.Authorization;
using SpaceProjectBackend.DTOs;
using SpaceProjectBackend.Enums;
using System.Security.Claims;
using SpaceProjectBackend.Utils;

namespace SpaceProjectBackend.Endpoints
{
    public static class TheoryEndpoints
    {
        public static void ConfigureTheoryEndpoints(this WebApplication app)
        {
            var TheoryGroup = app.MapGroup("theories");

            TheoryGroup.MapPost("/", CreateTheory);
            TheoryGroup.MapGet("/", GetTheories);
            TheoryGroup.MapGet("/{Id}", GetTheory);
            TheoryGroup.MapPut("/{Id}", UpdateTheory);
            TheoryGroup.MapDelete("/{Id}", DeleteTheory);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> CreateTheory(
            TheoryPostPayload payload,
            ITheoryRepository TheoryRepository, 
            IAuthRepository authRepository,
            ClaimsPrincipal user)
        {

            var uid = user.UserId();
            // string role = user.UserRole().ToString();
            // var email = user.UserEmail();

            if (uid == null)
            {
                return Results.Unauthorized();
            }
            
            if (payload.Name == "" || payload.Description == "" || payload.Usernotes == "")
            {
                return Results.BadRequest("Non-empty fields are required");
            }

            if (payload.Name == null || payload.Usernotes == null || payload.Description == null )
            {
                return Results.BadRequest("Non-null fields are required");
            }

            Theory? Theory = await TheoryRepository.CreateTheory(payload.Name, payload.Description, payload.Usernotes);

            if (Theory == null)
            {
                return Results.BadRequest("Failed to create Theory.");
            }

          
            TheoryRepository.SaveChanges();

            TheoryDTO mdto = new TheoryDTO(Theory);

            return TypedResults.Created($"/Theorys/{Theory.Id}", mdto);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize()]
        public static async Task<IResult> GetTheories(ITheoryRepository repository)
        {

            var res = await repository.GetTheories();

            List<TheoryDTO> reslist = new List<TheoryDTO>();

            foreach (Theory bk in res)
            {
                TheoryDTO dt = new TheoryDTO(bk);

                reslist.Add(dt);
            }

            return TypedResults.Ok(reslist);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> UpdateTheory(string TheoryId, TheoryUpdatePayload payload, ITheoryRepository repository, ClaimsPrincipal user)
        {

         
            if (payload.Name == null || payload.Usernotes == null || payload.Description == null)
            {
                return Results.BadRequest("Non-null fields are required");
            }


            Theory? ogTheory = await repository.GetTheory(TheoryId, PreloadPolicy.PreloadRelations);

            if (ogTheory == null)
            {
                return Results.BadRequest("Theory not found");
            }

            string newTitle = (payload.Name.Length > 0) ? payload.Name : ogTheory.Name;

            string newDescription = (payload.Description.Length > 0) ? payload.Description : ogTheory.Description;


            string newUsernotes = (payload.Usernotes.Length > 0) ? payload.Usernotes : ogTheory.Usernotes;


            Theory? Theory = await repository.UpdateTheory(ogTheory.Id, newTitle, newDescription, newUsernotes);

            if (Theory == null)
            {
                return Results.BadRequest("Failed to create Message.");
            }


            TheoryDTO mv = new TheoryDTO(Theory);


            repository.SaveChanges();

            return TypedResults.Created($"/Theorys/{Theory.Id}", mv);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> GetTheory(string TheoryId, ITheoryRepository repository)
        {

            Theory? Theory = await repository.GetTheory(TheoryId, PreloadPolicy.PreloadRelations);

            if (Theory == null)
            {
                return Results.NotFound("Theory not found");
            }

            var TheoryDTO = new TheoryDTO(Theory);

            repository.SaveChanges();

            return TypedResults.Ok(TheoryDTO);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> DeleteTheory(string TheoryId, ITheoryRepository repository, ClaimsPrincipal user)
        {

            Theory? Theory = await repository.DeleteTheory(TheoryId, PreloadPolicy.PreloadRelations);

            if (Theory == null)
            {
                return Results.NotFound("Theory not found");
            }

            var theoryDTO = new TheoryDTO(Theory);

            repository.SaveChanges();

            return TypedResults.Ok(theoryDTO);
        }
    }
}
