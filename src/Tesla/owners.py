import asyncio
from tesla_api import TeslaApiClient
import json
from datetime import date
from datetime import datetime

async def save_token(token):
    open("token_file.json", "w").write(token)

async def load_token():
    return open("token_file.json", "r").read()

async def main():
    #tokens =  """{"refresh_token":"eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Ilg0RmNua0RCUVBUTnBrZTZiMnNuRi04YmdVUSJ9.eyJpc3MiOiJodHRwczovL2F1dGgudGVzbGEuY29tL29hdXRoMi92MyIsImF1ZCI6Imh0dHBzOi8vYXV0aC50ZXNsYS5jb20vb2F1dGgyL3YzL3Rva2VuIiwiaWF0IjoxNjIyMzI0NzI1LCJzY3AiOlsib3BlbmlkIiwib2ZmbGluZV9hY2Nlc3MiXSwiZGF0YSI6eyJ2IjoiMSIsImF1ZCI6Imh0dHBzOi8vb3duZXItYXBpLnRlc2xhbW90b3JzLmNvbS8iLCJzdWIiOiIwYjQ1OGEzNi0yNmEzLTQ1ZDUtYjYzMS0wNjI0OTQxODFhMDYiLCJzY3AiOlsib3BlbmlkIiwiZW1haWwiLCJvZmZsaW5lX2FjY2VzcyJdLCJhenAiOiJvd25lcmFwaSIsImFtciI6WyJwd2QiLCJtZmEiLCJvdHAiXSwiYXV0aF90aW1lIjoxNjIyMzI0NzI0fX0.PJ_8rZnHK9n5BoLJLqfDFrbQW9si-nnxO_R5clMyXiyehaiXHbb3cSNz1GkZuOLKgVAJKur3jzMjptUXSLHpoNHGXEFNsOsmvlzGlY-mgtVio3U1DY0u1JPK7xnFf_ozgYwXUf5VDTE2YlzsoNWpI63whHkxPIM1SYiPt1-0OJDfzvtJYfnrwSgTB9SdApoM2vJhMhOyWJpGWgpqLVrJ6EOBS386rGlbnGxjNiwAA07SYNVOsTrDndr6YGgI7Atpo3EjhGo-b7afYlquhFT7cKgxpZwj-TYVvwT1eo6M4ouBQIBCVg2aogNgir2R-06Mjy9av68ZPrJx9qDWEt8rpRsT1K_fYcWrZUFmQ08Hh4ItvtfYLp_8usn3EmBfvbGeOusKtKwJyU26etEDMUdJ7g_mT1JB3Q5ElBm9SyfU2fWw3nHIoZbNfU2_QXsEncupuGwreBBCl_Haw7km5eQShTOQ7NbHnVCcyO9TEqsjuBr4U3RKoKD8FdcGjOxpptV0xPbICZZ3odHIoKgNhXnzAcHnMwxG1l-jJvEWMxig55UAbL-48x9kJ81SRhS6PGDN0EP7uqbidxWwt0CZ4yrRxv8TmS__9BR1bQg5Shv8LREpjc1ZhbtZ3VCbXv_dlm2OrSYx6e3AA89kV_R6LU6Ca9N1NUQEbAWZLM_jDlTtlNg",
     #               "access_token":"eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Ilg0RmNua0RCUVBUTnBrZTZiMnNuRi04YmdVUSJ9.eyJpc3MiOiJodHRwczovL2F1dGgudGVzbGEuY29tL29hdXRoMi92MyIsImF1ZCI6WyJodHRwczovL293bmVyLWFwaS50ZXNsYW1vdG9ycy5jb20vIiwiaHR0cHM6Ly9hdXRoLnRlc2xhLmNvbS9vYXV0aDIvdjMvdXNlcmluZm8iXSwiYXpwIjoib3duZXJhcGkiLCJzdWIiOiIwYjQ1OGEzNi0yNmEzLTQ1ZDUtYjYzMS0wNjI0OTQxODFhMDYiLCJzY3AiOlsib3BlbmlkIiwiZW1haWwiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsicHdkIiwibWZhIiwib3RwIl0sImV4cCI6MTYyMjM1MzUyNSwiaWF0IjoxNjIyMzI0NzI1LCJhdXRoX3RpbWUiOjE2MjIzMjQ3MjR9.T7yvSRvJlnu2AZB83J_juRQA8Uow2fUTgkAP6oYwhRbWEFzMtKyxwNlLYNDJyKnnDW1kiB1q90SdHQERmvvklav_V61NxyrL7IQky_pXSI-aS7Dtp8N3tM6_EpgP_dEWG3aSBhVlZcO9cNJRYMD_rgF7G_MZ0mJXaKFB5T28UNuHvDmNaeJVx5yWDApecfG8dA1MaekstnmDgXkNciV6e8w0Ieuft_dAfNPiPYnuDQqWUcrtX5jAtppQ_ypLLVqCr_BoxTE-LnFon9Zc7zHkuF6yjRY1ma2Dz07i1pyL4D0524d8tJXhP0Eyx9BdvYlh5CqAQMcFxIEsnZvgqLGgEoEg-l08drV0HppPhqTxQNOpcWI0zdLO-uSv1v8GZiRNgiC20WXftxvQQRYF6dX9HeAAj40S_wC7zJWTC8kW4qcetDHXczb7fABVu1hXiGh-hFxDGzfOlM_azJUrvwK9JU-Q7xcrqCc1vMFeyj-IBtN8MAiF0liYi8OuKWvnGGoH05WAvOepkghsiok5yA4mWFB84c24vRIfn0X61uVF6dN_arXM0_tQ4FI2l5_rw5C1ofrVYVN-Z4jO3fRrjqMpmMpvURO8ysx7lsCc8cQ_kuuitpIVGPeRzR8oqeinCt4p3zGsXuBXhkaDOChBYrHLpH9B_cwoMDjRzru9V_7XH-A",
      #              "expires_in":28800}"""
    
    #o = {}
    #o["resfresh_token"] = ""
    #o["access_token"] = ""
    #o["expires_in"]=28800
    #o["created_at"] = datetime.timestamp(datetime.now())
    #tokens = json.dumps(o)
    #await save_token(tokens)
    #exit()
    tokens = await load_token()    

    client = TeslaApiClient(token=tokens, on_new_token=save_token)

    energy_sites = await client.list_energy_sites()
    print("Number of energy sites = %d" % (len(energy_sites)))
    assert(len(energy_sites)==1)
    reserve = await energy_sites[0].get_backup_reserve_percent()
    print("Backup reserve percent = %d" % (reserve))
    print("Increment backup reserve percent")
    #await energy_sites[0].set_backup_reserve_percent(0)
    
    await client.close()

asyncio.run(main())