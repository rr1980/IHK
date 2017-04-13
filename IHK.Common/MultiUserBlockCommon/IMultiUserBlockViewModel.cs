using System;
using System.Collections.Generic;

namespace IHK.Common.MultiUserBlockCommon
{
    public interface IMultiUserBlockViewModel
    {
        Enum Command { get; set; }
        string SocketId { get; set; }
        EntityType EntityType { get; set; }
        int UserId { get; set; }
        int EntityId { get; set; }
        int Position { get; set; }
        string Name { get; set; }
        string Vorname { get; set; }
        string Description { get; set; }
        string Telefon { get; set; }
        List<IMultiUserBlockViewModel> Waits { get; set; }
    }
}
