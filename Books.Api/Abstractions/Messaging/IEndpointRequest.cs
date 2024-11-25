using MediatR;

namespace Books.Api.Abstractions.Messaging;

public interface IEndpointRequest : IRequest<IResult>;
