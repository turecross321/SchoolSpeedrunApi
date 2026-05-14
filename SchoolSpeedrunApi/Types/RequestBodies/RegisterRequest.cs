using SchoolSpeedrunApi.Types.Enums;

namespace SchoolSpeedrunApi.Types.RequestBodies;

public record RegisterRequest(string CardGuid, string Username, SchoolProgram SchoolProgram);