Feature: Game Flow
  As a player
  I want to play through the prompts
  So that I can experience the different levels of intimacy

Scenario: Progress to the next prompt
  Given I have started a game with 1 question per level
  When I click the prompt card
  Then the current prompt state should be 'Interlinked'

Scenario: Progress through all levels to game over
  Given I have started a game with 1 question per level
  When I click the prompt card 3 times
  Then I should see the game over screen

Scenario: Return to main menu from game over
  Given I have started a game
  And I have reached the game over screen
  When I click the 'main menu' button
  Then I should see the main menu