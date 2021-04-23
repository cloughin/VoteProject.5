UPDATE `vote`.`IssueGroups` 
SET `SubHeading`='Biographical,<br />General Philosophy,<br />Goals, Achievements' 
WHERE `IssueGroupKey`='LLREASONSOBJ';

INSERT INTO `vote`.`IssueGroupsIssues` (`IssueGroupKey`, `IssueKey`, `IssueOrder`) VALUES ('LLREASONSOBJ', 'ALLBio', '0');

INSERT INTO `vote`.`Issues` (`IssueKey`, `IssueOrder`, `Issue`, `IssueLevel`, `StateCode`, `CountyCode`, `LocalCode`, `IsIssueOmit`)
VALUES ('ALLBio', '5', 'Biographical', 'A', 'LL', '', '', '0');

ALTER TABLE `vote`.`Questions` 
CHANGE COLUMN `Question` `Question` VARCHAR(150) NOT NULL ;

INSERT INTO `vote`.`Questions` (`QuestionKey`, `IssueKey`, `QuestionOrder`, `Question`, `IsQuestionOmit`) VALUES ('ALLBio111111', 'ALLBio', '10', 'General (political statement of goals, objectives, views, philosophies)', '0');
INSERT INTO `vote`.`Questions` (`QuestionKey`, `IssueKey`, `QuestionOrder`, `Question`, `IsQuestionOmit`) VALUES ('ALLBio222222', 'ALLBio', '20', 'Personal (gender, age, marital status, spouse\'s name and age, children\'s name and ages, home town, current residence)', '0');
INSERT INTO `vote`.`Questions` (`QuestionKey`, `IssueKey`, `QuestionOrder`, `Question`, `IsQuestionOmit`) VALUES ('ALLBio333333', 'ALLBio', '30', 'Profession (professional and work experience outside politics)', '0');
INSERT INTO `vote`.`Questions` (`QuestionKey`, `IssueKey`, `QuestionOrder`, `Question`, `IsQuestionOmit`) VALUES ('ALLBio444444', 'ALLBio', '40', 'Civic (past and present organizations, charities involvement)', '0');
INSERT INTO `vote`.`Questions` (`QuestionKey`, `IssueKey`, `QuestionOrder`, `Question`, `IsQuestionOmit`) VALUES ('ALLBio555555', 'ALLBio', '50', 'Political (dates and titles of previously held political offices)', '0');
INSERT INTO `vote`.`Questions` (`QuestionKey`, `IssueKey`, `QuestionOrder`, `Question`, `IsQuestionOmit`) VALUES ('ALLBio666666', 'ALLBio', '60', 'Religion (current and past religious affiliations, beliefs)', '0');
INSERT INTO `vote`.`Questions` (`QuestionKey`, `IssueKey`, `QuestionOrder`, `Question`, `IsQuestionOmit`) VALUES ('ALLBio777777', 'ALLBio', '70', 'Accomplishments (significant accomplishments, awards, achievements)', '0');
INSERT INTO `vote`.`Questions` (`QuestionKey`, `IssueKey`, `QuestionOrder`, `Question`, `IsQuestionOmit`) VALUES ('ALLBio888888', 'ALLBio', '80', 'Education (times and places of schools, colleges, major, degrees, activities, sports)', '0');
INSERT INTO `vote`.`Questions` (`QuestionKey`, `IssueKey`, `QuestionOrder`, `Question`, `IsQuestionOmit`) VALUES ('ALLBio999999', 'ALLBio', '90', 'Military (branch, years of service, active duty experience, highest rank, medals, honors, type and date of discharge)', '0');

DELETE FROM `vote`.`Answers` WHERE IssueKey='ALLBio';

INSERT INTO `vote`.`Answers`
(`PoliticianKey`,
`QuestionKey`,
`StateCode`,
`IssueKey`,
`Answer`)
SELECT `Politicians`.`PoliticianKey`,
    'ALLBio111111',
    `Politicians`.`StateCode`,
	'ALLBio',
    `Politicians`.`GeneralStatement`
FROM `vote`.`Politicians` WHERE TRIM(`Politicians`.`GeneralStatement`)<>''

INSERT INTO `vote`.`Answers`
(`PoliticianKey`,
`QuestionKey`,
`StateCode`,
`IssueKey`,
`Answer`)
SELECT `Politicians`.`PoliticianKey`,
    'ALLBio222222',
    `Politicians`.`StateCode`,
	'ALLBio',
    `Politicians`.`Personal`
FROM `vote`.`Politicians` WHERE TRIM(`Politicians`.`Personal`)<>''

INSERT INTO `vote`.`Answers`
(`PoliticianKey`,
`QuestionKey`,
`StateCode`,
`IssueKey`,
`Answer`)
SELECT `Politicians`.`PoliticianKey`,
    'ALLBio333333',
    `Politicians`.`StateCode`,
	'ALLBio',
    `Politicians`.`Profession`
FROM `vote`.`Politicians` WHERE TRIM(`Politicians`.`Profession`)<>''

INSERT INTO `vote`.`Answers`
(`PoliticianKey`,
`QuestionKey`,
`StateCode`,
`IssueKey`,
`Answer`)
SELECT `Politicians`.`PoliticianKey`,
    'ALLBio444444',
    `Politicians`.`StateCode`,
	'ALLBio',
    `Politicians`.`Civic`
FROM `vote`.`Politicians` WHERE TRIM(`Politicians`.`Civic`)<>''

INSERT INTO `vote`.`Answers`
(`PoliticianKey`,
`QuestionKey`,
`StateCode`,
`IssueKey`,
`Answer`)
SELECT `Politicians`.`PoliticianKey`,
    'ALLBio555555',
    `Politicians`.`StateCode`,
	'ALLBio',
    `Politicians`.`Political`
FROM `vote`.`Politicians` WHERE TRIM(`Politicians`.`Political`)<>''

INSERT INTO `vote`.`Answers`
(`PoliticianKey`,
`QuestionKey`,
`StateCode`,
`IssueKey`,
`Answer`)
SELECT `Politicians`.`PoliticianKey`,
    'ALLBio666666',
    `Politicians`.`StateCode`,
	'ALLBio',
    `Politicians`.`Religion`
FROM `vote`.`Politicians` WHERE TRIM(`Politicians`.`Religion`)<>''

INSERT INTO `vote`.`Answers`
(`PoliticianKey`,
`QuestionKey`,
`StateCode`,
`IssueKey`,
`Answer`)
SELECT `Politicians`.`PoliticianKey`,
    'ALLBio777777',
    `Politicians`.`StateCode`,
	'ALLBio',
    `Politicians`.`Accomplishments`
FROM `vote`.`Politicians` WHERE TRIM(`Politicians`.`Accomplishments`)<>''

INSERT INTO `vote`.`Answers`
(`PoliticianKey`,
`QuestionKey`,
`StateCode`,
`IssueKey`,
`Answer`)
SELECT `Politicians`.`PoliticianKey`,
    'ALLBio888888',
    `Politicians`.`StateCode`,
	'ALLBio',
    `Politicians`.`Education`
FROM `vote`.`Politicians` WHERE TRIM(`Politicians`.`Education`)<>''

INSERT INTO `vote`.`Answers`
(`PoliticianKey`,
`QuestionKey`,
`StateCode`,
`IssueKey`,
`Answer`)
SELECT `Politicians`.`PoliticianKey`,
    'ALLBio999999',
    `Politicians`.`StateCode`,
	'ALLBio',
    `Politicians`.`Military`
FROM `vote`.`Politicians` WHERE TRIM(`Politicians`.`Military`)<>''
