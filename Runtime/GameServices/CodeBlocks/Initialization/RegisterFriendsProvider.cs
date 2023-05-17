using System.Threading.Tasks;
using Toolset;
using Unity.Services.Friends;
using Unity.Services.Friends.Options;
using UnityEngine;

namespace GameServices.CodeBlocks
{
    [CreateAssetMenu(fileName = "Register Friends Provider", menuName = "Code Blocks/Initialization/Register Friends Provider", order = 0)]
    public class RegisterFriendsProvider : CodeBlock
    {
        protected override void Execute()
        {
            InitializeFriendsService();
        }

        private async void InitializeFriendsService()
        {
            var options = new InitializeOptions();
            options.WithMemberProfile(true).WithEvents(true).WithMemberPresence(true);

            await FriendsService.Instance.InitializeAsync(options);

            if (Services.All.Single<IFriendsProvider>() == null)
                Services.All.RegisterSingle<IFriendsProvider>(new FriendsProvider());


            Complete(true);
        }
    }
}