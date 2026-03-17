using SchoolSpeedrunApi.Types.Database;

namespace SchoolSpeedrunApi.Types.ResponseBodies;

public record SubmitLocationResponse(DbLocation SubmittedLocation, DbRun? Run);