import os


def file_len(fname):
    with open(fname) as f:
        for i, l in enumerate(f):
            pass
    return i + 1


totalScripts = 0
totalLines = 0
pairs = []
for root, dirs, files in os.walk("Scripts"):
    for file in files:
        if file.endswith('.cs'):
            totalScripts += 1
            count = file_len(root + "/" + file)
            totalLines += count
            pairs.append((file, count))
            # print(file + "\t" + str(count))

pairs.sort(key=lambda v: v[1])

for pair in pairs:
    print(str(pair[1]) + "\t  " + pair[0])

importless = totalLines - totalScripts * 5
spaceless = int(totalLines * (3 / 4))

print()
print("Total scripts : " + str(totalScripts))
print("Total Lines : " + str(totalLines))
print("Lines without imports : " + str(importless) + " (estimated)")
print("Lines without imports or spaces : " + str(spaceless) + " (estimated)")
print("---------------------")
print("Average Lines : " + str(totalLines / totalScripts))
print("Median Lines : " + str(pairs[int(len(pairs) / 2)][1]))
