using SchoolSpeedrunApi.Types.Enums;

namespace SchoolSpeedrunApi.Types.RequestBodies;

public record RegisterRequest(Guid RegistrationGuid, string Username, SchoolProgram SchoolProgram);