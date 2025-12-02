//using System;
//using MediatR;

//namespace medicine_box_api.Domain.Utils;
//public abstract class Command<TResponse> : Message, IRequest<CommandResponse<TResponse>>, IBaseRequest
//{
//    public DateTime Timestamp { get; private set; }

//    public CommandResponse<TResponse> CommandResponse { get; set; }

//    protected Command()
//    {
//        Timestamp = DateTime.Now;
//    }
//}
