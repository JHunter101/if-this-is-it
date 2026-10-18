Feature: Initialization
  As a player
  I want to start the application
  So that I can see the main menu and begin a session

Scenario: View Main Menu on load
  Given I am on the home screen
  Then I should see the main menu

Scenario: Start a new game
  Given I am on the home screen
  When I click the 'are we really strangers?' button
  Then the game play view should be visible
  And the current prompt state should be 'Strangers'