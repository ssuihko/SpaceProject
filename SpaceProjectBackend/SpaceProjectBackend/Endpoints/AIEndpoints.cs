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
    public static class AIEndpoints
    {
        public static void ConfigureAIEndpoints(this WebApplication app)
        {
            var AIGroup = app.MapGroup("ais");

            AIGroup.MapPost("/", CreateAI);
            AIGroup.MapGet("/", GetAIs);
            AIGroup.MapGet("/{Id}", GetAI);
            AIGroup.MapPut("/{Id}", UpdateAI);
            AIGroup.MapDelete("/{Id}", DeleteAI);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> CreateAI(
            AIPostPayload payload,
            IAIRepository AIRepository, 
            IPersonRepository personRepository,
            IAuthRepository authRepository,
            ClaimsPrincipal user)
        {

            var uid = user.UserId();

            if (uid == null)
            {
                return Results.Unauthorized();
            }

            Console.WriteLine("ID of the current user..?");
            Console.WriteLine(uid);

            Person? person = await personRepository.GetPerson(payload.CreatorId);


            // we need to be able to send this down the line as null...
            // if (person == null) {
            //    return Results.BadRequest("Creator not found with id");
            //}
            
            if (payload.Name == "" || payload.Description == "" || payload.Usernotes == "")
            {
                return Results.BadRequest("Non-empty fields are required");
            }

            if (payload.Name == null || payload.Usernotes == null || payload.Description == null )
            {
                return Results.BadRequest("Non-null fields are required");
            }

            Console.WriteLine("Creating AI in endpoints...");

            AI? AI = await AIRepository.CreateAI(
                payload.Name, 
                payload.Description, 
                payload.Real,
                payload.CreatorId, 
                payload.Image, 
                payload.Usernotes);

            if (AI == null)
            {
                return Results.BadRequest("Failed to create AI.");
            }

          
            AIRepository.SaveChanges();

            AIDTO mdto = new AIDTO(AI);

            return TypedResults.Created($"/AIs/{AI.Id}", mdto);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize()]
        public static async Task<IResult> GetAIs(IAIRepository repository)
        {

            var res = await repository.GetAIs();

            List<AIDTO> reslist = new List<AIDTO>();

            foreach (AI bk in res)
            {
                AIDTO dt = new AIDTO(bk);

                reslist.Add(dt);
            }

            return TypedResults.Ok(reslist);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> UpdateAI(string AIId, AIUpdatePayload payload, IAIRepository repository, ClaimsPrincipal user)
        {

            AI? ogAI = await repository.GetAI(AIId, PreloadPolicy.PreloadRelations);

            if (ogAI == null)
            {
                return Results.BadRequest("AI not found");
            }

            string newName = (payload.Name != null && payload.Name.Length > 0) ? payload.Name : ogAI.Name;

            string newDescription = (payload.Description != null && payload.Description.Length > 0) ? payload.Description : ogAI.Description;

            bool newReal = ogAI.Real;

            if (newReal != payload.Real) {
                newReal = !ogAI.Real;
            }

            string newCreatorId = null;
            if (payload.CreatorId != null) {
                newCreatorId = (payload.CreatorId.Length > 0) ? payload.CreatorId : ogAI.CreatorId;
            }

            string newImage = (payload.Image.Length > 0) ? payload.Image : ogAI.Image;

            string newUsernotes = (payload.Usernotes.Length > 0) ? payload.Usernotes : ogAI.Usernotes;


            AI? AI = await repository.UpdateAI(
                ogAI.Id, 
                newName, 
                newDescription,
                newReal, 
                newCreatorId,
                newImage,
                newUsernotes);

            if (AI == null)
            {
                return Results.BadRequest("Failed to create Message.");
            }


            AIDTO mv = new AIDTO(AI);


            repository.SaveChanges();

            return TypedResults.Created($"/AIs/{AI.Id}", mv);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static async Task<IResult> GetAI(string AIId, IAIRepository repository)
        {

            AI? AI = await repository.GetAI(AIId, PreloadPolicy.PreloadRelations);

            if (AI == null)
            {
                return Results.NotFound("AI not found");
            }

            var AIDTO = new AIDTO(AI);

            repository.SaveChanges();

            return TypedResults.Ok(AIDTO);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()] // [Authorize(Roles = "Manager")] 
        public static async Task<IResult> DeleteAI(string AIId, IAIRepository repository, ClaimsPrincipal user)
        {

            AI? AI = await repository.DeleteAI(AIId, PreloadPolicy.PreloadRelations);

            if (AI == null)
            {
                return Results.NotFound("AI not found");
            }

            var AIDTO = new AIDTO(AI);

            repository.SaveChanges();

            return TypedResults.Ok(AIDTO);
        }
    }
}
