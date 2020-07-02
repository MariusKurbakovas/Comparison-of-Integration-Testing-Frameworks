Feature: DiscountTests

Scenario: Submit form and get correct results
	Given I open http://localhost:3000/
	And I input 20 to 'Originali kaina:'
	And I select 'Maxima' in 'Lojalumo tipas:'
	And I select 'Studentas' in 'Kliento tipas:'
	When I click submit
	Then Results appear in 3000 miliseconds
	And Price is 18

Scenario: Submit form and get correct results in table
	Given I open http://localhost:3000/
	And I input 20 to 'Originali kaina:'
	And I select 'Maxima' in 'Lojalumo tipas:'
	And I select 'Studentas' in 'Kliento tipas:'
	When I click submit
	Then Results appear in 3000 miliseconds
	Then Results appear in table
	And The 2 column is 20
	And The 3 column is Maxima
	And The 4 column is Studentas
	And The 5 column is 0
	And The 6 column is 18
	And The 7 column is 0