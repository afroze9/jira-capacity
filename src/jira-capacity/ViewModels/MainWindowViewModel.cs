using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using DynamicData;
using jira_capacity.Models;
using jira_capacity.Services;
using ReactiveUI;

namespace jira_capacity.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly JiraService _jiraService;

    private SprintResponse _selectedSprint;
    
    public ReactiveCommand<Unit, Unit> LoadSprintsCommand { get; }
    public ReactiveCommand<Unit, Unit> LoadSprintIssuesCommand { get; }
    public ObservableCollection<SprintResponse> Sprints { get; }

    public SprintResponse SelectedSprint
    {
        get => _selectedSprint;
        set => this.RaiseAndSetIfChanged(ref _selectedSprint, value);
    }

    public MainWindowViewModel()
    {
        _jiraService = new JiraService();
        Sprints = new ObservableCollection<SprintResponse>();
        
        LoadSprintsCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            List<SprintResponse> sprints = await _jiraService.GetData();
            Sprints.Clear();
            Sprints.AddRange(sprints);
        });

        LoadSprintIssuesCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (_selectedSprint == null)
            {
                return;
            }
            
            var issues = await _jiraService.GetSprintIssues(_selectedSprint.Id);
        });
    }
}
