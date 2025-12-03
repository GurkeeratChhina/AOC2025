import requests
import os
import re
from datetime import datetime
from zoneinfo import ZoneInfo

class Client:
    def __init__(self, session_id):
        self.session = requests.Session()
        self.session.cookies.set('session', session_id)
        
        now = datetime.now(tz = ZoneInfo("EST"))

        self.year = 2025
        if now.year > self.year or (now.year == self.year and now.day >=12):
            self.day = 12
        else:
            self.day = now.day

    def get_input(self):

        for day in range (1, self.day+1):
            # check if input file exists
            if not os.path.isfile(f"AdventOfCode/Inputs/{day:02d}.txt"):
                print(f"Getting input data for day {day}")
                url = f'https://adventofcode.com/{self.year}/day/{day}/input'
                response = self.session.get(url)
                with open(f"AdventOfCode/Inputs/{day:02d}.txt", 'w') as f:
                    f.write(response.text)
            # check if solution file exists
            if not os.path.isfile(f"AdventOfCode/Day{day:02d}.cs"):
                print(f"Generating template file for day {day}")
                with open("AdventOfCode/Inputs/template.txt") as template, open(f"AdventOfCode/Day{day:02d}.cs", 'w') as newfile:
                    # copy template line by line, allowing regex to replace references to the day with the appropriate number
                    for line in template:
                        output = re.sub("Day01", f'Day{day:02d}', re.sub(r'd1',f'd{day:02d}', line))
                        newfile.write(output)

            

def main():
    with open("AdventOfCode/Inputs/session.txt") as f:
        lines = f.readlines()
        sess_id = lines[0].strip()

    client = Client(sess_id)
    client.get_input()

if __name__ == '__main__':
    main()