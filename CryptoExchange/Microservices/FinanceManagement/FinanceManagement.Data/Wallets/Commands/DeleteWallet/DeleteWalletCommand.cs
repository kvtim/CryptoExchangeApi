using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Data.Wallets.Commands.DeleteWallet
{
    public class DeleteWalletCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
