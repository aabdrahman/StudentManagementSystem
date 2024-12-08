
using StudentManagementApplication.Interfaces;
using StudentManagementApplication.Services;

internal class StudentManagementBackgroundService : BackgroundService
{
    private readonly ILogger<StudentManagementBackgroundService> _logger;


    public StudentManagementBackgroundService(ILogger<StudentManagementBackgroundService> logger)
    {
        _logger = logger;
       // _studentManagementService = studentManagementService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"Started at: {DateTime.Now}");
            var _timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            if (await _timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation($"Running task at: {DateTime.Now}");
                //var resp = await _studentManagementService.GetCourses();
                _logger.LogInformation($"Run Success");
            }

            await Task.Delay(10000, stoppingToken);
        }
    }
}