using SafeSchool.Api.Features.Wallet.Common;

namespace SafeSchool.Api.Features.Wallet.Sync;

public sealed class OfflinePosReserveService
{
    public OperationResult<string> Validate(long purchaseAmountMinor, long perStudentReserveMinor, long perTerminalReserveMinor)
    {
        if (purchaseAmountMinor > perStudentReserveMinor) return OperationResult<string>.Failure(new ValidationError("student_reserve_exceeded", "Offline purchase exceeds the student reserve snapshot."));
        if (purchaseAmountMinor > perTerminalReserveMinor) return OperationResult<string>.Failure(new ValidationError("terminal_reserve_exceeded", "Offline purchase exceeds the terminal reserve snapshot."));
        return OperationResult<string>.Success("reserve-ok");
    }
}
