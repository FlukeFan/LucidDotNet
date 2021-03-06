﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Lucid.Lib.Facade.Exceptions;
using Lucid.Modules.AppFactory.Design.Contract;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class Blueprint : Registry.Entity
    {
        protected Blueprint() { }

        public virtual int      OwnedByUserId   { get; protected set; }
        public virtual string   Name            { get; protected set; }

        public static async Task<Blueprint> StartAsync(StartEditCommand cmd)
        {
            var blueprint = new Blueprint
            {
                OwnedByUserId = cmd.OwnedByUserId,
            };

            blueprint.Update(cmd);
            return await Repository.SaveAsync(blueprint);
        }

        public static async Task<Blueprint> EditAsync(StartEditCommand cmd)
        {
            var blueprint = await Repository.LoadAsync<Blueprint>(cmd.BlueprintId);
            blueprint.Update(cmd);
            return blueprint;
        }

        public virtual BlueprintDto ToDto()
        {
            return new BlueprintDto
            {
                Id = Id,
                Name = Name,
            };
        }

        protected virtual void Update(StartEditCommand cmd)
        {
            if (OwnedByUserId != cmd.OwnedByUserId)
                throw new Exception("Invalid attempt to change the owner of a Blueprint");

            VerifyNameIsUnique(Id, OwnedByUserId, cmd.Name);
            Name = cmd.Name;
        }

        private static void VerifyNameIsUnique(int blueprintId, int ownedByUserId, string name)
        {
            var existingBlueprint = Repository.Query<Blueprint>()
                .Where(bp => bp.Id != blueprintId)
                .Where(bp => bp.OwnedByUserId == ownedByUserId)
                .Where(bp => bp.Name == name)
                .SingleOrDefault();

            if (existingBlueprint != null)
                throw new FacadeException($"There is already a Blueprint with the name '{existingBlueprint.Name}'");
        }
    }
}
