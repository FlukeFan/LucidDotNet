﻿using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;
using Lucid.Infrastructure.Lib.Facade.Exceptions;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class StartEditCommand : CommandAsync<int>
    {
        public int BlueprintId      { get; set; }
        public int OwnedByUserId    { get; set; }

        [Required(ErrorMessage = "Please supply a Name")]
        public string Name { get; set; }

        public override async Task<int> ExecuteAsync()
        {
            if (OwnedByUserId == 0)
                throw new FacadeException("User id not specified");

            var blueprint = (BlueprintId == 0)
                ? await Blueprint.StartAsync(this)
                : await Blueprint.EditAsync(this);

            return blueprint.Id;
        }
    }
}
