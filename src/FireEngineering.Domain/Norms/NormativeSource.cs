namespace FireEngineering.Domain.Norms;

public sealed record NormativeSource(
    string DocumentId,
    string Edition,
    string Clause,
    string TableOrAppendix,
    string Url,
    DateOnly VerifiedOn,
    string VerificationStatus);
