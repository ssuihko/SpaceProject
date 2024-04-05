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
    public static class SpacecraftEndpoints
    {
        public static void ConfigureSpacecraftEndpoints(this WebApplication app)
        {
            var SpacecraftGroup = app.MapGroup("spacecrafts");

            SpacecraftGroup.MapPost("/", CreateSpacecraft);
            SpacecraftGroup.MapGet("/", GetSpacecrafts);
            SpacecraftGroup.MapGet("/{Id}", GetSpacecraft);
            SpacecraftGroup.MapPut("/{Id}", UpdateSpacecraft);
            SpacecraftGroup.MapDelete("/{Id}", DeleteSpacecraft);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> CreateSpacecraft(
            SpacecraftPostPayload payload,
            ISpacecraftRepository SpacecraftRepository,
            IPersonRepository personRepository,
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

            Person? person = await personRepository.GetPerson(payload.CreatorId);

            if (payload.Name == null || payload.Usernotes == null || payload.Description == null )
            {
                return Results.BadRequest("Non-null fields are required");
            }

            Spacecraft? Spacecraft = await SpacecraftRepository.CreateSpacecraft(
                payload.Name, 
                payload.Real,
                payload.Description,
                payload.CreatorId, 
                payload.Image, 
                payload.Usernotes);

            if (Spacecraft == null)
            {
                return Results.BadRequest("Failed to create spacecraft.");
            }

          
            SpacecraftRepository.SaveChanges();

            SpacecraftDTO mdto = new SpacecraftDTO(Spacecraft);

            return TypedResults.Created($"/spacecrafts/{Spacecraft.Id}", mdto);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize()]
        public static async Task<IResult> GetSpacecrafts(ISpacecraftRepository repository)
        {

            var res = await repository.GetSpacecrafts();

            List<SpacecraftDTO> reslist = new List<SpacecraftDTO>();

            foreach (Spacecraft spc in res)
            {
                SpacecraftDTO dt = new SpacecraftDTO(spc);

                reslist.Add(dt);
            }

            return TypedResults.Ok(reslist);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> UpdateSpacecraft(string SpacecraftId, SpacecraftUpdatePayload payload, ISpacecraftRepository repository, ClaimsPrincipal user)
        {


            if (payload.Name == null || payload.Usernotes == null || payload.Description == null)
            {
                return Results.BadRequest("Non-null fields are required");
            }


            Spacecraft? ogSpacecraft = await repository.GetSpacecraft(SpacecraftId, PreloadPolicy.PreloadRelations);

            if (ogSpacecraft == null)
            {
                return Results.BadRequest("Spacecraft not found");
            }

            string newName = (payload.Name.Length > 0) ? payload.Name : ogSpacecraft.Name;

            string newDescription = (payload.Description.Length > 0) ? payload.Description : ogSpacecraft.Description;

            bool newReal = ogSpacecraft.Real;

            if (payload.Real != null && newReal != payload.Real) {
                newReal = (bool)payload.Real;
            } 

          
            string newCreatorId = ((payload.CreatorId != null) && (payload.CreatorId.Length > 0)) ? payload.CreatorId : ogSpacecraft.CreatorId;
     
            
            string newImage = (payload.Image.Length > 0) ? payload.Image : ogSpacecraft.Image;

            string newUsernotes = (payload.Usernotes.Length > 0) ? payload.Usernotes : ogSpacecraft.Usernotes;


            Spacecraft? spc = await repository.UpdateSpacecraft(
                ogSpacecraft.Id, 
                newName,
                newReal, 
                newDescription,
                newCreatorId,
                newImage,
                newUsernotes);

            if (spc == null)
            {
                return Results.BadRequest("Failed to create Message.");
            }


            SpacecraftDTO mv = new SpacecraftDTO(spc);

            repository.SaveChanges();

            return TypedResults.Created($"/AIs/{spc.Id}", mv);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> GetSpacecraft(string SpacecraftId, ISpacecraftRepository repository)
        {

            Spacecraft? Spacecraft = await repository.GetSpacecraft(SpacecraftId, PreloadPolicy.PreloadRelations);

            if (Spacecraft == null)
            {
                return Results.NotFound("Spacecraft not found");
            }

            var SpacecraftDTO = new SpacecraftDTO(Spacecraft);

            repository.SaveChanges();

            return TypedResults.Ok(SpacecraftDTO);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> DeleteSpacecraft(string SpacecraftId, ISpacecraftRepository repository, ClaimsPrincipal user)
        {

            Spacecraft? Spacecraft = await repository.DeleteSpacecraft(SpacecraftId, PreloadPolicy.PreloadRelations);

            if (Spacecraft == null)
            {
                return Results.NotFound("Spacecraft not found");
            }

            var spacecraftDTO = new SpacecraftDTO(Spacecraft);

            repository.SaveChanges();

            return TypedResults.Ok(spacecraftDTO);
        }
    }
}
