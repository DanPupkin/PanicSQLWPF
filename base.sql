DROP TABLE IF EXISTS students CASCADE;
DROP TABLE IF EXISTS matan;
DROP TABLE IF EXISTS algebra;
DROP TABLE IF EXISTS proga;
CREATE TABLE students
(
	student_id SERIAL PRIMARY KEY,
	student_name text,
	student_disciplines text[],
	password_md5 text
	
);
CREATE TABLE matan
(

	student_id int REFERENCES students(student_id) ON DELETE CASCADE,
	discipline_status bool DEFAULT false
);
CREATE TABLE algebra
(

	student_id int REFERENCES students(student_id) ON DELETE CASCADE,
	discipline_status bool DEFAULT false
);
CREATE TABLE proga
(

	student_id int REFERENCES students(student_id) ON DELETE CASCADE,
	discipline_status bool DEFAULT false
);
INSERT INTO students(student_name, student_disciplines, password_md5)
VALUES 
('pavel', '{"matan, algebra, proga"}', 'e10adc3949ba59abbe56e057f20f883e'),
('ivan', '{"matan, algebra, proga"}', 'e10adc3949ba59abbe56e057f20f883e'),
('joe', '{"matan, algebra, proga"}', 'e10adc3949ba59abbe56e057f20f883e');

INSERT INTO matan (student_id, discipline_status)
 VALUES 
 (1, true),
 (2,true),
 (3,false);
 INSERT INTO algebra (student_id, discipline_status)
 VALUES 
 (1, false),
 (2,true),
 (3,true);
 INSERT INTO proga (student_id, discipline_status)
 VALUES 
 (1, false),
 (2,false),
 (3,false);
 SELECT matan.discipline_status AS matan, algebra.discipline_status, proga.discipline_status
FROM students
JOIN matan ON students.student_id = matan.student_id JOIN algebra ON students.student_id = algebra.student_id
JOIN proga ON students.student_id = proga.student_id
WHERE student_name = 'pavel';
SELECT student_name, password_md5 FROM students WHERE student_name = 'pavel' AND password_md5 = 'e10adc3949ba59abbe56e057f20f883e';