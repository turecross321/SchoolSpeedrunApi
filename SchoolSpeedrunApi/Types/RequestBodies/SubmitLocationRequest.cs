using SchoolSpeedrunApi.Types.Enums;

namespace SchoolSpeedrunApi.Types.RequestBodies;

public record SubmitLocationRequest(Position Position, string CardGuid, DateTime Date);